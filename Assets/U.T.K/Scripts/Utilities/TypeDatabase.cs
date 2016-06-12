using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace UTK.Utilities
{
    public class TypeDatabase<U>
    {
        protected Dictionary<Type, Dictionary<int, List<U>>> typeDictionaryByID = new Dictionary<Type, Dictionary<int, List<U>>>();
        protected Dictionary<Type, List<U>> typeDictionary = new Dictionary<Type, List<U>>();

        public List<U> this[int id, Type type]
        {
            get
            {
                if (id >= 0)
                {
                    if (typeDictionaryByID.ContainsKey(type))
                    {
                        if (typeDictionaryByID[type].ContainsKey(id))
                        {
                            return typeDictionaryByID[type][id];
                        }
                    }
                }
                else
                {
                    if(typeDictionary.ContainsKey(type))
                    {
                        return typeDictionary[type];
                    }
                }

                return null;
            }
        }


        public TypeDatabase()
        {
            typeDictionary = new Dictionary<Type, List<U>>();
            typeDictionaryByID = new Dictionary<Type, Dictionary<int, List<U>>>();
        }

        public virtual void Add(U newObject, int id = -1)
        {
            //Get the type of the newObject
            Type objectType = newObject.GetType();

            //Check if the type is registered in the type dictionary
            if (!typeDictionary.ContainsKey(objectType))
            {
                typeDictionaryByID.Add(objectType, new Dictionary<int, List<U>>());
                typeDictionary.Add(objectType, new List<U>());
            }

            if(!typeDictionaryByID[objectType].ContainsKey(id))
            {
                typeDictionaryByID[objectType].Add(id, new List<U>());
            }

            typeDictionary[objectType].Add(newObject);
            typeDictionaryByID[objectType][id].Add(newObject);
        }

        /// <summary>
        /// Removes an object of type T from the Type database if it exists.
        /// If no ID is specified, then the entire database will be searched for
        /// instances of T object. Otherwise, only instances of T object that
        /// also fit the ID specified will be removed. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToRemove"></param>
        /// <param name="id"></param>
        public virtual void Remove(U objectToRemove, int id = -1)
        {
            Type objectType = objectToRemove.GetType();
            if(id < 0)
            {
                if(!typeDictionaryByID.ContainsKey(objectType))
                {
                    return;
                }

                foreach(int key in typeDictionaryByID[objectType].Keys)
                {
                    if(typeDictionaryByID[objectType][key].Contains(objectToRemove))
                        typeDictionaryByID[objectType][key].Remove(objectToRemove);
                }
            }
            else
            {
                if (!typeDictionaryByID.ContainsKey(objectType))
                {
                    return;
                }
                else if (!typeDictionaryByID[objectType].ContainsKey(id))
                {
                    return;
                }

                if (typeDictionaryByID[objectType][id].Contains(objectToRemove))
                    typeDictionaryByID[objectType][id].Remove(objectToRemove);
            }
        }

        public virtual List<List<U>> GetListsOfAllSubTypes<T>(int id = -1, bool typeInclusive = true)
            where T : U
        {
            Assembly assembly = Assembly.GetAssembly(typeof(U));
            Type baseType = typeof(T);

            List<List<U>> subTypeLists = new List<List<U>>();

            foreach (Type type in typeDictionaryByID.Keys)
            {
                //This type is inherited from the base type
                if(type.IsAssignableFrom(baseType))
                {
                    if(id < 0)
                    {
                        foreach (int index in typeDictionaryByID[type].Keys)
                            subTypeLists.Add(typeDictionaryByID[type][index]);
                    }
                }
            }

            return subTypeLists;
        }

        public virtual List<T> GetListOfType<T>(int id = -1)
            where T : U
        {
            Type objectType = typeof(T);

            return this[id, objectType].Cast<T>().ToList();
        }

        public virtual T GetObjectAtIndex<T>(int index, int id = -1)
            where T : U
        {
            Type objectType = typeof(T);

            List<U> objects = this[id, objectType];
            
            if(objects.Count >= index)
            {
                return (T)objects[index];
            }

            return default(T);
        }
    }
}
