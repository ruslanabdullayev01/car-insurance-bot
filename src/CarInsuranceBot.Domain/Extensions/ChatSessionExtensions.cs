using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class ChatSessionExtensions
    {
        private static readonly HashSet<long> ActiveUsers = [];

        public static void StartChat(long userId) => ActiveUsers.Add(userId);
        public static void EndChat(long userId) => ActiveUsers.Remove(userId);
        public static bool IsActive(long userId) => ActiveUsers.Contains(userId);
    }
}
