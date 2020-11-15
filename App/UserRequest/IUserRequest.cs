using System;

namespace App.BotTask
{
    public interface IUserRequest
    {
        Action Task { get; }
    }
}