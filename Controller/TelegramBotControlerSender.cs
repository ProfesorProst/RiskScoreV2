using DependencyCheck.Models;
using RiskScore.Models;
using RiskScore.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace RiskScore.Controller
{
    class TelegramBotControlerSender
    {
        ModelUser modelUser;
        TelegramBotView view;
        public Telegram.Bot.TelegramBotClient bot;
        public TelegramBotControlerSender(Telegram.Bot.TelegramBotClient bot)
        {
            modelUser = new ModelUser();
            view = new TelegramBotView();
            this.bot = bot;
        }

        internal string TextMessage(long userId, Message message)
        {
            string rezult = "";
            if (message.Text == "/start")
                if (message.Chat.Username == "" || message.Chat.Username == null) SendOneMessage(userId, view.createNewUserEmptyUsername, null);
                else
                {
                    UserDB person = modelUser.CreateNewUser(userId, message.Chat.Username);
                    SendOneMessage(userId, view.createNewUserEmptyUsername, null);
                    rezult = view.createNewUserSucces(person);
                }
                else SendOneMessage(userId, "What? I dont understand. Write /start", null);
            return rezult;
        }

        internal string CallbackQuery(long userId, string data, Message message)
        {
            String s1;
            if (modelUser.TryIfExist(userId))
                if (data == "Ready")
                {
                    FindNewWork(userId);
                }
                else
                if (new Regex(@"^(([1-9]|10),(threats|techDamage|bizDamage),\d*)$").IsMatch(data))
                {
                    var splitedText = data.Split(",");
                    int mark = Convert.ToInt32(splitedText[0]);
                    long vulnId = Convert.ToInt32(splitedText[2]);

                    modelUser.UserCreateMark(mark, splitedText[1], vulnId, userId);
                    SendOneMessage(userId, view.nothingToDo, null);
                }

            return "";
        }

        private bool FindNewWork(long userId)
        {
            if (modelUser.FindEmptyVuln())
            {
                SendOneMessage(userId, view.nothingToDo, null);
            }
            else
            {
                var vuln = modelUser.FindTask(userId);
                if (vuln == null)
                    SendOneMessage(userId, view.nothingToDo, null);
                else
                {
                    SendOneInlineMessage(userId, view.textAboutVuln(vuln), view.setMark(vuln));
                    return false;
                }
            }
            return true;
        }

        internal void SendStartMessagesToAll()
        {
            SendMessagAll( view.checkIfReady, view.inlineKeyboard);
        }

        private void SendMessagToMaster(String s)
        {
            SendOneMessage(413595040, s, null);
        }

        private void SendOneMessage(long id, String s, Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardMarkup)
        {
            try
            {
                bot.SendTextMessageAsync(id, s, Telegram.Bot.Types.Enums.ParseMode.Default, true, false, 0, keyboardMarkup);
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) { SendMessagToMaster(ex.ToString()); }
        }

        private void SendTwoMessages(long id, String s1, String s2, Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardMarkup)
        {
            try
            {
                bot.SendTextMessageAsync(id, s1, Telegram.Bot.Types.Enums.ParseMode.Default, true, false, 0, keyboardMarkup);
                bot.SendTextMessageAsync(id, s2, Telegram.Bot.Types.Enums.ParseMode.Default, true, false, 0, keyboardMarkup);
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) { SendMessagToMaster(ex.ToString()); }
        }

        private void SendOneInlineMessage(long id, String s, Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup keyboardMarkup)
        {
            try
            {
                bot.SendTextMessageAsync(id, s, Telegram.Bot.Types.Enums.ParseMode.Default, true, false, 0, keyboardMarkup);
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) { SendMessagToMaster(ex.ToString()); }
        }

        private void SendMessagAll(String s, Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup keyboardMarkup)
        {
            List<UserDB> people = modelUser.GetObjects();
            foreach (UserDB person in people)
                SendOneInlineMessage(person.id, s, keyboardMarkup);
        }
    }
}
