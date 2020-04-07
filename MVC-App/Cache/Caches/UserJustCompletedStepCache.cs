using System.Collections.Generic;

namespace MVC_App.Cache.Caches
{
    public class UserJustCompletedStepCache : IUserJustCompletedStepCache
    {
        public Dictionary<string,string> _usersJustCompletedAction;

        public UserJustCompletedStepCache()
        {
            _usersJustCompletedAction = new Dictionary<string, string>();
        }

        public void AddUser(string userId, string schemaId)
        {
            _usersJustCompletedAction.Add(userId, schemaId);
        }

        public void RemoveUser(string userId)
        {
            _usersJustCompletedAction.Remove(userId);
        }

        public bool ContainsUser(string userId)
        {
            return _usersJustCompletedAction.ContainsKey(userId);
        }

        public string GetSchema(string userId)
        {
            if (_usersJustCompletedAction.ContainsKey(userId))
            {
                return _usersJustCompletedAction[userId];
            }
            return null;
        }

        public bool UserAndSchemaStored(string userId, string schemaId)
        {
            return _usersJustCompletedAction.ContainsKey(userId) && _usersJustCompletedAction[userId] == schemaId;
        }
    }
}
