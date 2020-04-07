//using Siccar.Shallow.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Siccar.CacheManager.Caches
//{
//    public class ProcessSchemaCacheV1 : IProcessSchemaCache
//    {
//        private Dictionary<string, HashSet<ProcessSchema>> _processSchema;


//        public ProcessSchemaCacheV1()
//        {
//            _processSchema = new Dictionary<string, HashSet<ProcessSchema>>();
//        }

//        public bool ContainsAll(string userId, HashSet<ProcessSchema> schemas)
//        {
//            var key = _processSchema.ContainsKey(userId);
//            if (key)
//            {
//                var storedScheams = _processSchema[userId].ToList();
//                var listOfSchemas = schemas.ToList();

//                try
//                {
//                    var isEqual = storedScheams.SequenceEqual(listOfSchemas);
//                    return key && isEqual;
//                }
//                catch (Exception)
//                {
//                    // A NPE is thrown from the getHashCode methods if the Tx is not set, on this occasion we should update again
//                }

//            }
//            return false;
//        }

//        public HashSet<ProcessSchema> GetSchemas(string userId)
//        {
//            if (_processSchema.ContainsKey(userId))
//            {
//                return _processSchema[userId];
//            }
//            else
//            {
//                return new HashSet<ProcessSchema>();
//            }
//        }

//        public void AddSchemas(string userId, HashSet<ProcessSchema> schemas)
//        {
//            if (_processSchema.ContainsKey(userId))
//            {
//                var hash = _processSchema[userId];

//                var listSchemas = schemas.ToList();

//                foreach (var schema in listSchemas)
//                {
//                    var found = _processSchema[userId].ToList().Find(x => x.schemaId == schema.schemaId && x.schemaInstanceId == schema.schemaInstanceId);
//                    if (found != null)
//                    {
//                        // Replace incase steps have changed
//                        hash.Remove(found);
//                    }
//                    hash.Add(schema);
//                }
//                _processSchema[userId] = hash;
//            }
//            else
//            {
//                _processSchema.Add(userId, schemas);
//            }
//        }

//        public void AddSchema(string userId, ProcessSchema schema)
//        {
//            if (_processSchema.ContainsKey(userId))
//            {
//                var hashsets = _processSchema[userId];
//                hashsets.Add(schema);
//                _processSchema[userId] = hashsets;
//            }
//            else
//            {
//                _processSchema.Add(userId, new HashSet<ProcessSchema>() { schema });
//            }
//        }

//    }
//}
