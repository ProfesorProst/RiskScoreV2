using DependencyCheck.Models;
using RiskScore.Models;
using RiskScore.View;
using System;
using System.Collections.Generic;
using System.Text;
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
            /*
            if (modelPerson.TryIfExist(userId))
            {
                if (data == "Elf" || data == "Orc" || data == "Person" || data == "Gnome")
                {
                    s1 = "Gods do not want you to change the race!";
                    SendOneMessage(userId, s1, view.keyboardHome);
                }
                else
                if (data == "attack" || data == "def")
                {
                    int attackOrDef = 0;
                    if (data == "def") attackOrDef = 1;
                    if (modelPerson.LvlUp(userId, attackOrDef)) s1 = view.upStatesOK;
                    else s1 = view.upStatesFalse;
                    SendOneMessage(userId, s1, view.keyboardHome);
                }
                else
                if ((data == "Alliance" || data == "Republic") && modelPerson.GetPerson(userId).fraction == null)
                {
                    int allianceOrRepublic = 0;
                    if (data == "Republic") allianceOrRepublic = 1;
                    if (modelPerson.SetFraction(userId, allianceOrRepublic)) s1 = view.setFractionSuccses;
                    else s1 = view.setFractionFail;
                    SendOneMessage(userId, s1, view.keyboardHome);
                }
                else
                if (new Regex(@"^Accept ").IsMatch(data))
                {
                    Guild guild = modelGuild.GuildJoinOut(userId, Convert.ToInt64(data.Split(' ')[1]));
                    if (guild != null)
                    {
                        SendOneMessage(userId, viewGuild.GetGuild(guild), viewGuild.keyboardGuild);
                        Person person = modelPerson.GetObjectByPersonNick(guild.master);
                        SendOneMessage(person.id, viewGuild.inviteAccepted, viewGuild.keyboardGuild);
                    }
                }
            }

            else
            if (data == "Elf" || data == "Orc" || data == "Person" || data == "Gnome")
            {
                if (message.Chat.Username == "" || message.Chat.Username == null) SendOneMessage(userId, view.createNewUserEmptyUsername, view.keyboardHome);
                else
                {
                    Person person = modelPerson.CreateNewUser(userId, message.Chat.Username, data);
                    s1 = "You hav choosen " + data;
                    String s2 = view.States(person, modelPerson.atackAdditional(person.id), modelPerson.defAdditional(person.id));
                    SendTwoMessages(userId, s1, s2, view.keyboardHome);
                }
            }
            */
            return "";
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
