﻿using DependencyCheck.Entity;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RiskScore.Controller
{
    class TelegramBotControler
    {
        TelegramBotClient bot;
        TelegramBotControlerSender controlerSender;
        public TelegramBotControler()
        {
            bot = new Telegram.Bot.TelegramBotClient("1058823676:AAEVt1nLbSlwbgeH0urg4n9AZIzyd3yxwmY"); //533785870:AAEjN0SJJs02eMIO3rgL6IiUWhzz7-NMkeg
            controlerSender = new TelegramBotControlerSender(bot);
        }

        public async void bw_DoWork(Ref<string> rezult)
        {
            try
            {
                 bot.SetWebhookAsync("");

                bot.OnUpdate += async (object su, Telegram.Bot.Args.UpdateEventArgs evu) =>
                {
                    if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return; // в этом блоке нам келлбэки и инлайны не нужны
                    var update = evu.Update;
                    var message = update.Message;
                    if (message == null) return;
                    long userId = message.Chat.Id;
                    if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                        controlerSender.TextMessage(userId, message, ref rezult);
                };

                // Callback'и от кнопок
                bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
                {
                    var message = ev.CallbackQuery.Message;
                    long userId = ev.CallbackQuery.Message.Chat.Id;
                    var data = ev.CallbackQuery.Data;

                    controlerSender.CallbackQuery(userId, data, message, ref rezult);
                };

                bot.StartReceiving();
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {

            }

            //e.Result = result;///!!!
        }

        public int GetAllEmptyVulnerabilitiesCount()
        {
            return controlerSender.GetAllEmptyVulnerabilitiesCount();
        }

        internal int UserCount()
        {
            return controlerSender.UserCount();
        }

        internal void SendStartMessagesToAll()
        {
            controlerSender.SendStartMessagesToAll();
        }
    }
}
