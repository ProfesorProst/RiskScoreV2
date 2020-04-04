using DependencyCheck.Models;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace RiskScore.View
{
    class TelegramBotView
    {
        internal string createNewUserEmptyUsername;
        internal string createNewUserSucces;
        internal string checkIfReady;
        internal string nothingToDo;  
        public InlineKeyboardMarkup inlineKeyboard;
        public TelegramBotView()
        {
            createNewUserSucces = "Your welcom our hero!";
            createNewUserEmptyUsername = "Please set your Username in the settings of Telegram!";
            checkIfReady = "Hello new Hero! We was waiting for you!";
            nothingToDo = "We have all information from you, thanks. See you later, when I will need you help)";

            inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Ready", "Ready"),
                    }
                });
        }

        internal string createdNewUser(UserDB person)
        {
            return "User " + person.username + " was added to DB.";
        }

        internal string textAboutVuln(UserVulnerabilityDB vulnerabilityDB)
        {
            return "Vulnerability name: " + vulnerabilityDB.vulnerability.name+"\n" +
                    "Description: " + vulnerabilityDB.vulnerability.description;
        }

        internal InlineKeyboardMarkup setMark(UserVulnerabilityDB vulnerabilityDB)
        {
            var numberToInput = (vulnerabilityDB.threats == null) ? "threats" : (vulnerabilityDB.techDamage == null) ? "techDamage" :
                (vulnerabilityDB.bizDamage == null) ? "bizDamage" : "";
            numberToInput += "," + vulnerabilityDB.vulnerability.name;
            return new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1", "1," + numberToInput),
                        InlineKeyboardButton.WithCallbackData("2", "2," + numberToInput),
                        InlineKeyboardButton.WithCallbackData("3", "3," + numberToInput),
                        InlineKeyboardButton.WithCallbackData("4", "4," + numberToInput),
                        InlineKeyboardButton.WithCallbackData("5", "5," + numberToInput),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("6", "6," + numberToInput),
                        InlineKeyboardButton.WithCallbackData("7", "7," + numberToInput),
                        InlineKeyboardButton.WithCallbackData("8", "8," + numberToInput),
                        InlineKeyboardButton.WithCallbackData("9", "9," + numberToInput),
                        InlineKeyboardButton.WithCallbackData("10", "10," + numberToInput),
                    },
                }); ;
        }
    }
}
