using System.Text;
using backend.Models;
using Newtonsoft.Json;
using System.Net.Http;
public class FlutterwaveService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _secretKey;
    private readonly ILogger<FlutterwaveService> _logger;

    public FlutterwaveService(HttpClient httpClient, IConfiguration config,ILogger<FlutterwaveService> logger)
    {
        _httpClient = httpClient;
        _baseUrl = config["Flutter:BaseUrl"];
        _secretKey = config["Flutter:Secret_key"];
        _logger = logger;
        
    }

    // Method to initialize a payment
    public async Task<string> InitializePayment(decimal amount, Attendee attendee, int eventId, HttpRequest request)
    {
        Console.WriteLine(amount);
        var redirectUrl = GenerateRedirectUrl(eventId, request);
        Console.WriteLine(redirectUrl);
        var paymentData = new
        {
            tx_ref = GenerateUUID(),
            amount = amount,
            currency = "NGN",
            redirect_url = redirectUrl,
            customer = new
            {
                email = attendee.Email,
                name = attendee.FirstName,
                phone_number = attendee.PhoneNumber,
                id = attendee.Id
            }
          
        };
        
        // Serialize payment data to JSON
        var json = JsonConvert.SerializeObject(paymentData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Set the authorization header for the request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _secretKey);

        // Make the HTTP POST request to the Flutterwave payment endpoint
        var response = await _httpClient.PostAsync(_baseUrl, content);
        response.EnsureSuccessStatusCode(); // Throw if not a success code.

        // Read and return the response content
        var result = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response>(result);

        // Return only the link
        return responseObject?.Data?.Link; // Optionally parse the response to get the payment link if needed
        }

        public class Response
        {
            public Data? Data { get; set; }
        }

        public class Data
        {
            public string? Link { get; set; }
        }

        private string GenerateRedirectUrl(int eventId, HttpRequest request)
        {
        return $"{request.Scheme}://{request.Host}/attendee/api/confirm/ticket/payment/{eventId}";
    
        }
          private static string GenerateUUID()
        {
            return Guid.NewGuid().ToString();
        }
}
