namespace ForumBE.Middlewares
{
    public class HandlingValidation
    {
        private readonly RequestDelegate _next;

        public HandlingValidation(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                context.Request.EnableBuffering(); // Cho phép đọc nhiều lần
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset stream về đầu

                //if (string.IsNullOrWhiteSpace(body))
                //{
                //    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                //    context.Response.ContentType = "application/json";

                //    var response = new { status = 400, message = "Fill all field" };
                //    await context.Response.WriteAsJsonAsync(response);
                //    return;
                //}
            }

            await _next(context);
        }
    }

}
