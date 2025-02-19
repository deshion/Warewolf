#pragma warning disable
/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Dev2
{
    public static class CustomContainer
    {
        public static List<Type> LoadedTypes { get; set; }

        static readonly Dictionary<Type, object> RegisterdTypes = new Dictionary<Type, object>();
        static readonly Dictionary<Type, Func<object>> RegisterdPerRequestTypes = new Dictionary<Type, Func<object>>();

        public static int EntiresCount => RegisterdTypes.Count;


        public static void Clear()
        {
            RegisterdTypes.Clear();
        }

        public static void Register<T>(T concrete)
        {
            if (RegisterdTypes.ContainsKey(typeof(T)))
            {
                DeRegister<T>();
            }

            RegisterdTypes.Add(typeof(T), concrete);
        }

        public static T Get<T>() where T : class
        {
            var requestedType = typeof(T);
            if (RegisterdTypes.ContainsKey(requestedType))
            {
                var registerdType = RegisterdTypes[requestedType];
                return registerdType as T;
            }

            return null;
        }

        public static object Get(Type type)
        {
            var requestedType = type;
            if (RegisterdTypes.ContainsKey(requestedType))
            {
                var registerdType = RegisterdTypes[requestedType];
                return registerdType;
            }

            return null;
        }

        public static void DeRegister<T>()
        {
            if (RegisterdTypes.ContainsKey(typeof(T)))
            {
                RegisterdTypes.Remove(typeof(T));
            }
        } 
        
        public static void AddToLoadedTypes(Type type)
        {
            if (LoadedTypes is null)
            {
                LoadedTypes = new List<Type>();
            }

            if (!LoadedTypes.Contains(type))
            {
                LoadedTypes.Add(type);
            }
        }

        public static T CreateInstance<T>(params object[] constructorParameters)
        {
            var typeToCreate = typeof(T);
            var assemblyTypes = LoadedTypes ?? new List<Type>();
            object createdObject = null;
            foreach (var assemblyType in assemblyTypes.Where(a => a != null))
            {
                if (assemblyType.IsPublic && !assemblyType.IsAbstract && assemblyType.IsClass &&
                    !assemblyType.IsGenericType && typeToCreate.IsAssignableFrom(assemblyType))
                {
                    createdObject = TryInvokeConstructor(assemblyType, constructorParameters);
                }
            }

            if (createdObject != null)
            {
                return (T)createdObject;
            }

            return default(T);
        }

        static object TryInvokeConstructor(Type assemblyType, object[] constructorParameters)
        {
            object createdObject = null;
            var constructorInfos = assemblyType.GetConstructors();
            foreach (var constructorInfo in constructorInfos)
            {
                if (ConstructorMatch(constructorParameters, constructorInfo) && createdObject == null)
                {
                    createdObject = constructorInfo.Invoke(constructorParameters);
                }
            }

            return createdObject;
        }

        static bool ConstructorMatch(object[] constructorParameters, System.Reflection.ConstructorInfo constructorInfo)
        {
            var constructorMatch = false;
            var parameterInfos = constructorInfo.GetParameters();
            var numberOfParameters = parameterInfos.Length;
            if (numberOfParameters == constructorParameters.Length)
            {
                for (int i = 0; i < numberOfParameters; i++)
                {
                    var constructorParameterType = parameterInfos[i].ParameterType;
                    var givenParameterType = constructorParameters[i].GetType();
                    if ((givenParameterType == constructorParameterType) ||
                        constructorParameterType.IsAssignableFrom(givenParameterType))
                    {
                        constructorMatch = true;
                    }
                    else
                    {
                        constructorMatch = false;
                        break;
                    }
                }
            }

            return constructorMatch;
        }

        public static void RegisterInstancePerRequestType<T>(Func<object> constructorFunc)
        {
            if (RegisterdPerRequestTypes.ContainsKey(typeof(T)))
            {
                DeRegisterInstancePerRequestType<T>();
            }

            RegisterdPerRequestTypes.Add(typeof(T), constructorFunc);
        }

        public static T GetInstancePerRequestType<T>() where T : class
        {
            var requestedType = typeof(T);
            if (RegisterdPerRequestTypes.ContainsKey(requestedType))
            {
                var registerdType = RegisterdPerRequestTypes[requestedType];
                return registerdType.Invoke() as T;
            }

            return null;
        }

        static void DeRegisterInstancePerRequestType<T>()
        {
            if (RegisterdPerRequestTypes.ContainsKey(typeof(T)))
            {
                RegisterdPerRequestTypes.Remove(typeof(T));
            }
        }
    }
}