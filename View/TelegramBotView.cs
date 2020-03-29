using DependencyCheck.Models;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace RiskScore.View
{
    class TelegramBotView
    {
        internal string createNewUserEmptyUsername = "Please set your Username in the settings of Telegram!";
        internal string checkIfReady = "Hello new Hero! We was waiting for you!";
        public InlineKeyboardMarkup inlineKeyboard;
        public TelegramBotView()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Ready", "Ready"),
                    }
                });
        }

        internal string createNewUserSucces(UserDB person)
        {
            return "User " + person.username + " was added to DB.";
        }
    }
}
