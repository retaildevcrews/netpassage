// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static NetPassage.KeyVaultBase;

namespace NetPassage
{
    public static class KeyVaultConfigExtensions
    {
        public static IConfigurationBuilder AddAzureKeyVaultConfiguration(this IConfigurationBuilder configBuilder, string keyVaultSettingName)
        {
            // build a temporary configuration so we can extract the key vault urls
            if (configBuilder == null)
            {
                throw new ArgumentNullException(nameof(configBuilder));
            }

            // build the configuration so we merge the configuration providers into an IConfiguration
            var tmpConfig = configBuilder.Build();

            // get the config setting from IConfiguration, from whatever configuration provider specified the setting
            var keyVaultName = tmpConfig[keyVaultSettingName];

            if (string.IsNullOrWhiteSpace(keyVaultName))
            {
                throw new InvalidDataException($"Missing KeyVault configuration value {keyVaultSettingName}");
            }

            if (!KeyVaultHelper.BuildKeyVaultConnectionString(keyVaultName, out var keyVaultUrlSettingName))
            {
                throw new InvalidDataException("Key vault name not Valid");
            }

            configBuilder.AddAzureKeyVault(keyVaultUrlSettingName);

            return configBuilder;
        }

        public static IConfigurationBuilder AddAzureKeyVaultConfiguration(IConfigurationBuilder configBuilder, Uri keyVaultUrlSettingName)
        {
            throw new NotImplementedException();
        }
    }
}
