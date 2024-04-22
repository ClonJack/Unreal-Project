﻿using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace UnrealTeam.SB.Services.Factories
{
    public class ObjectInjector
    {
        public T Instantiate<T>(
            IObjectResolver objectResolver,
            T prefab)
            where T : Component
        {
            var component = Object.Instantiate(prefab);
            objectResolver.InjectGameObject(component.gameObject);
            return component;
        }

        public T Instantiate<T>(
            IObjectResolver objectResolver,
            T prefab,
            Transform parent,
            bool worldPositionStays = false)
            where T : Component
        {
            var component = Object.Instantiate(prefab, parent, worldPositionStays);
            objectResolver.InjectGameObject(component.gameObject);
            return component;
        }

        public T Instantiate<T>(
            IObjectResolver objectResolver,
            T prefab,
            Vector3 position,
            Quaternion rotation)
            where T : Component
        {
            var component = Object.Instantiate(prefab, position, rotation);
            objectResolver.InjectGameObject(component.gameObject);
            return component;
        }

        public T Instantiate<T>(
            IObjectResolver objectResolver,
            T prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
            where T : Component
        {
            var component = Object.Instantiate(prefab, position, rotation, parent);
            objectResolver.InjectGameObject(component.gameObject);
            return component;
        }

        public GameObject Instantiate(
            IObjectResolver objectResolver,
            GameObject prefab)
        {
            var obj = Object.Instantiate(prefab);
            objectResolver.InjectGameObject(obj);
            return obj;
        }

        public GameObject Instantiate(
            IObjectResolver objectResolver,
            GameObject prefab,
            Transform parent,
            bool worldPositionStays = false)
        {
            var obj = Object.Instantiate(prefab, parent, worldPositionStays);
            objectResolver.InjectGameObject(obj);
            return obj;
        }

        public GameObject Instantiate(
            IObjectResolver objectResolver,
            GameObject prefab,
            Vector3 position,
            Quaternion rotation)
        {
            var obj = Object.Instantiate(prefab, position, rotation);
            objectResolver.InjectGameObject(obj);
            return obj;
        }

        public GameObject Instantiate(
            IObjectResolver objectResolver,
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
        {
            var obj = Object.Instantiate(prefab, position, rotation, parent);
            objectResolver.InjectGameObject(obj);
            return obj;
        }
    }
}