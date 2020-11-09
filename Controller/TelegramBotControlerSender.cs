using DependencyCheck.Entity;
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
        ModelVulnerabilityDB modelVulnerabilityDB;
        TelegramBotView view;
        public Telegram.Bot.TelegramBotClient bot;
        public TelegramBotControlerSender(Telegram.Bot.TelegramBotClient bot)
        {
            modelUser = new ModelUser();
            modelVulnerabilityDB = new ModelVulnerabilityDB();
            view = new TelegramBotView();
            this.bot = bot;
        }

        internal void TextMessage(long userId, Message message,ref Ref<string> rezult)
        {
            if (!modelUser.TryIfExist(userId))
                if (message.Text == "/start")
                if (message.From.Username == "" || message.From.Username == null) SendOneMessage(userId, view.createNewUserEmptyUsername, null);
                else
                {
                    UserDB person = modelUser.CreateNewUser(userId, message.Chat.Username);
                    SendOneMessage(userId, view.createNewUserSucces, null);
                    rezult.Value = view.createdNewUser(person);
                }
                else SendOneMessage(userId, "What? I dont understand. Write /start", null);
            return ;
        }

        internal void CallbackQuery(long userId, string data, Message message, ref Ref<string> rezult)
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
                    string categotry = splitedText[1];

                    var uservul = modelVulnerabilityDB.UserCreateMark(mark, categotry, vulnId, userId);
                    FindNewWork(userId);
                    if(uservul!=null)
                    rezult.Value = "+1," + " User: " + userId + ". vuler: " + uservul.vulnerability.name +
                        ". Added: " + categotry;
                }
        }

        internal int UserCount()
        {
            return modelUser.GetObjects().Count;
        }

        private bool FindNewWork(long userId)
        {
            if (!modelVulnerabilityDB.FindEmptyVuln())
            {
                SendOneMessage(userId, view.nothingToDo, null);
            }
            else
            {
                UserVulnerabilityDB vuln = modelVulnerabilityDB.FindTask(userId);
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

        public int GetAllEmptyVulnerabilitiesCount()
        {
            return modelVulnerabilityDB.GetAllEmptyVulnerabilities().Count();
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
