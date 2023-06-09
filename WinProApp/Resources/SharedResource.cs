﻿using Microsoft.Extensions.Localization;
namespace WinProApp.Resources
{
    public interface ISharedResource
    {
    }

    public class SharedResource : ISharedResource
    {
        private readonly IStringLocalizer _localizer;

        public SharedResource(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string this[string index] => _localizer[index];
    }
}
