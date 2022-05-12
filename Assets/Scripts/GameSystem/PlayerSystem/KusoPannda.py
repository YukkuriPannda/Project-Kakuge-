
import discord
TOKEN = ""
client = discord.Client()
@client.event
async def on_ready():
    print("ピピッ！ログイン完了")

@client.event
async def on_message(message):
    if message.content == "/Hi!KusoPannda!":
        await message.channnel.send("Fuck" + message.author)