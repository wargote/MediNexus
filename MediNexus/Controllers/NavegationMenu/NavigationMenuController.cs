using MediNexus.Api.Contracts.NavegationMenus;
using MediNexus.Domain.Users;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers.NavegationMenu
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NavigationController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public NavigationController(MediNexusDbContext db)
        {
            _db = db;
        }

        // Opción A: tomar el usuario del token (recomendado)
        [HttpGet("me")]
        public async Task<ActionResult<NavigationResponse>> GetMyNavigation()
        {
            // asumiendo que en el token guardaste el UserId como claim "sub" o "id"
            var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("id");
            if (userIdClaim is null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            return await GetNavigationInternal(userId);
        }

        // Opción B: recibir userId por parámetro
        [Authorize(Roles = "Admin")] // por seguridad solo admin debería poder usar esto
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<NavigationResponse>> GetNavigation(int userId)
        {
            return await GetNavigationInternal(userId);
        }

        private async Task<ActionResult<NavigationResponse>> GetNavigationInternal(int userId)
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

            if (user is null)
                return NotFound("User not found or inactive.");

            var roleId = user.UserRoleId;

            // 1) Submenús permitidos por rol
            var subMenus = await (
                from sm in _db.NavigationSubMenus
                join rsm in _db.RoleSubMenuPermissions on sm.Id equals rsm.SubMenuId
                where sm.IsActive && rsm.RoleId == roleId && rsm.CanView
                select sm
            )
            .OrderBy(sm => sm.MenuId)
            .ThenBy(sm => sm.SortOrder)
            .ToListAsync();

            // 2) Menús permitidos por rol (aunque no tengan submenús)
            var roleMenuIds = await _db.RoleMenuPermissions
                .Where(rmp => rmp.RoleId == roleId && rmp.CanView)
                .Select(rmp => rmp.MenuId)
                .Distinct()
                .ToListAsync();

            // 3) Menús que aparecen por tener submenús permitidos
            var menuIdsFromSubMenus = subMenus
                .Select(sm => sm.MenuId)
                .Distinct()
                .ToList();

            // 4) Unión: menús permitidos por submenú + menús permitidos directamente
            var allowedMenuIds = roleMenuIds
                .Union(menuIdsFromSubMenus)
                .Distinct()
                .ToList();

            // 5) Traer menús activos permitidos
            var menus = await _db.NavigationMenus
                .Where(m => m.IsActive && allowedMenuIds.Contains(m.Id))
                .OrderBy(m => m.SortOrder)
                .ToListAsync();

            // 6) Módulos que contienen esos menús
            var moduleIds = menus
                .Select(m => m.ModuleId)
                .Where(id => id != null)
                .Distinct()
                .ToList();

            var modules = await _db.NavigationModules
                .Where(mod => mod.IsActive && moduleIds.Contains(mod.Id))
                .OrderBy(mod => mod.SortOrder)
                .ToListAsync();

            // 7) Proyección final: si el menú no tiene submenús, SubMenus queda vacío []
            var response = new NavigationResponse
            {
                Modules = modules.Select(mod => new ModuleDto
                {
                    Id = mod.Id,
                    Name = mod.Name,
                    SortOrder = mod.SortOrder,
                    Menus = menus
                        .Where(m => m.ModuleId == mod.Id)
                        .Select(m => new MenuDto
                        {
                            Id = m.Id,
                            Name = m.Name,
                            Route = m.Route,
                            Icon = m.Icon,
                            SortOrder = m.SortOrder,
                            SubMenus = subMenus
                                .Where(sm => sm.MenuId == m.Id)
                                .Select(sm => new SubMenuDto
                                {
                                    Id = sm.Id,
                                    Name = sm.Name,
                                    Route = sm.Route,
                                    SortOrder = sm.SortOrder
                                })
                                .ToList()
                        })
                        .ToList()
                }).ToList()
            };

            return Ok(response);
        }

    }

}
