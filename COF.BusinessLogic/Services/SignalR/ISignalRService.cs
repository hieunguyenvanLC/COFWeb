using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services.SignalR
{
    public interface ISignalRService
    {
        #region user

        /// <summary>
        /// add user async
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<bool> AddUserAsync(string userId, string connectionId);

        /// <summary>
        /// remove user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<bool> RemoveUserAsync(string userId, string connectionId);

        /// <summary>
        /// remove user
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<bool> RemoveUserAsync(string connectionId);

        /// <summary>
        /// Get all connections for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetListConnectionForUserAsync(string userId);
        /// <summary>
        /// Get all connections from connection Id
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<List<string>> GetListConnectionFromConnectionIdAsync(string connectionId);


        bool AddUser(string userId, string connectionId);

        bool RemoveUser(string userId, string connectionId);

        bool RemoveUser(string connectionId);

        bool IsConnected(string userId);

        List<string> GetListConnectionForUser(string userId);


        /// <summary>
        ///  Get current username mapping with connectionId
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        string GetCurrentUsernameOfConnection(string connectionId);

        #endregion


        #region group

        /// <summary>
        /// add connectionId to group async
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<bool> AddConnectionIdToGroupAsync(string groupname, string connectionId);

        /// <summary>
        /// add connectionId to group
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="connectionId"></param>
        bool AddConnectionIdToGroup(string groupname, string connectionId);


        /// <summary>
        /// remove connectionId from group async
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<bool> RemoveConnectionIdFromGroupAsync(string groupname, string connectionId);

        /// <summary>
        /// remove connectionId from group
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="connectionId"></param>
        bool RemoveConnectionIdFromGroup(string groupname, string connectionId);


        /// <summary>
        /// get list connectionIds of group
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        List<string> GetListConnectionIdsOfGroup(string groupname);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<bool> IsExistInGroupAsync(string groupname, string connectionId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        bool IsExistInGroup(string groupname, string connectionId);


        /// <summary>
        ///  Get current group id mapping with connectionId
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        string GetCurrentGroupIdOfConnection(string connectionId);
        #endregion
    }
}
