﻿using System;
using fastJSON;
using ModConfigEnforcer;
using UnityEngine;

namespace EpicLoot
{
    public class JsonFileConfigVariable<T> : IConfigVariable where T : class
    {
        private readonly string _sourceFile;
        private T _config;

        public JsonFileConfigVariable(string sourceFile)
        {
            _sourceFile = sourceFile;
        }

        public string GetName() => _sourceFile;
        public bool LocalOnly() => false;
        public Type GetValueType() => typeof(T);
        public T Value => (T)GetValue();

        public object GetValue()
        {
            return ConfigManager.ShouldUseLocalConfig ? EpicLoot.LoadJsonFile<T>(_sourceFile) : _config;
        }

        public void SetValue(object o)
        {
            // Will never be called
        }

        public void Serialize(ZPackage zpg)
        {
            Debug.LogWarning($"Serialized config ({_sourceFile})");
            var configJson = EpicLoot.LoadJsonText(_sourceFile);
            zpg.Write(configJson);
        }

        public bool Deserialize(ZPackage zpg)
        {
            Debug.LogWarning($"Deserialized config ({_sourceFile})");
            var configJson = zpg.ReadString();
            _config = JSON.ToObject<T>(configJson);
            return _config != null;
        }
    }
}
