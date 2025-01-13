using SoapCore.Extensibility;
using System.ServiceModel.Channels;

namespace Application.Soap
{
    public class SoapExceptionMessageProcessor : ISoapMessageProcessor
    {
        public async Task<Message> ProcessMessage(Message message, HttpContext context, Func<Message, Task<Message>> next)
        {
            //var bufferedMessage = message.CreateBufferedCopy(int.MaxValue);
            //var msg = bufferedMessage.CreateMessage();
            //var reader = msg.GetReaderAtBodyContents();
            //var content = reader.ReadInnerXml();

            //now you can inspect and modify the content at will.
            //if you want to pass on the original message, use bufferedMessage.CreateMessage(); otherwise use one of the overloads of Message.CreateMessage() to create a new message
            //var originalMessage = bufferedMessage.CreateMessage();

            //pass the modified message on to the rest of the pipe.
            var responseMessage = await next(message);


            
            //Inspect and modify the contents of returnMessage in the same way as the incoming message.
            //finish by returning the modified message.

            return responseMessage;
        }
    }
}
