﻿using System;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("unallow", "Revoke a user's permission to a private room. Only works if you're an owner of that room.", "user [room]", "room")]
    public class UnAllowCommand : UserCommand
    {
        public override void Execute(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidOperationException("Who you want to revoke access permissions from?");
            }

            string targetUserName = args[0];

            ChatUser targetUser = context.Repository.VerifyUser(targetUserName);

            string roomName = args.Length > 1 ? args[1] : callerContext.RoomName;

            if (String.IsNullOrEmpty(roomName))
            {
                throw new InvalidOperationException("Which room do you want to revoke access from?");
            }

            ChatRoom targetRoom = context.Repository.VerifyRoom(roomName, mustBeOpen: false);

            context.Service.UnallowUser(callingUser, targetUser, targetRoom);

            context.NotificationService.UnallowUser(targetUser, targetRoom);

            context.Repository.CommitChanges();
        }
    }
}