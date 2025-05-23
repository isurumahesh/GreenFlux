﻿using GreenFlux.Application.DTOs;

namespace GreenFlux.Application.Interfaces
{
    public interface IGroupService
    {
        Task<GroupDTO> SaveGroup(GroupCreateDTO groupDTO);

        Task UpdateGroup(Guid groupdId, GroupUpdateDTO groupDTO);

        Task<GroupDTO> GetGroup(Guid id);

        Task<List<GroupDTO>> GetAllGroups();

        Task DeleteGroup(Guid id);
    }
}