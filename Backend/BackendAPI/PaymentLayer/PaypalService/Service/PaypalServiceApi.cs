using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentLayer.PaypalService.Configuration;
using PaymentLayer.PaypalService.Interface;
using PaymentLayer.PaypalService.Model.RequestsDto;
using PaymentLayer.PaypalService.Model.ResponeDto;


namespace PaymentLayer.PaypalService.Service
{
    public class PaypalServiceApi : IPaypal
    {
        private readonly HttpClient _httpClient;

        public PaypalServiceApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateProductResponse> CreateProductAsync(CreatProductRequsetDto requset)
        {
            var tokenRespone = await GetAuthorizationRequestAsync();
            var requestContent= JsonConvert.SerializeObject(requset);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConfigurationHelper.BaseUrl}/v1/catalogs/products")
            {
                Content=new StringContent(requestContent,Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer",tokenRespone.access_token);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var respone= await _httpClient.SendAsync(httpRequestMessage);
            var responeAsString=await respone.Content.ReadAsStringAsync();
            if (!respone.IsSuccessStatusCode)
                throw new Exception($"خطأ أثناء إنشاء الخطة - Status: {respone.StatusCode}, Body: {responeAsString}");
            var result=JsonConvert.DeserializeObject<CreateProductResponse>(responeAsString);
            return result;
        
        }

        public async Task<bool> EditProduct(string productId, EditProductRequest request)
        {
            var Token = await GetAuthorizationRequestAsync();
            var requestContent = JsonConvert.SerializeObject(new List<EditProductRequest> { request });

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, $"{ConfigurationHelper.BaseUrl}/v1/catalogs/products/{productId}")
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue ("application/json"));
            var respone=await _httpClient.SendAsync(httpRequestMessage);
            var responeAsString= await respone.Content.ReadAsStringAsync();
            if (!respone.IsSuccessStatusCode)
                throw new Exception($"خطأ أثناء إنشاء الخطة - Status: {respone.StatusCode}, Body: {responeAsString}");
            return true;
        }


        public async Task<CreatePlanResponse> CreatePlanAsync(CreatePlanRequest request)
        {
            var accessToken = await GetAuthorizationRequestAsync();

            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConfigurationHelper.BaseUrl}/v1/billing/plans")
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(httpRequestMessage);

            var responseAsString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"خطأ أثناء إنشاء الخطة - Status: {response.StatusCode}, Body: {responseAsString}");

            var result = JsonConvert.DeserializeObject<CreatePlanResponse>(responseAsString);

            return result;
        }


        public async Task<CreateSubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request)
        {
            var Token = await GetAuthorizationRequestAsync();
            var requestContent = JsonConvert.SerializeObject(request);
            var HttpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConfigurationHelper.BaseUrl}/v1/billing/subscriptions")
            {
                Content=new StringContent(requestContent,Encoding.UTF8, "application/json")
            };
            HttpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);
            HttpRequestMessage.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
            var respone= await _httpClient.SendAsync(HttpRequestMessage);
            var responeAsString= await respone.Content.ReadAsStringAsync();
            if (!respone.IsSuccessStatusCode)
                throw new Exception($"خطأ أثناء إنشاء الخطة - Status: {respone.StatusCode}, Body: {responeAsString}");
            var result=JsonConvert.DeserializeObject<CreateSubscriptionResponse>(responeAsString);

            return result;

        }

        public async Task<AuthorizationResponseData> GetAuthorizationRequestAsync()
        {
            var byteArry = Encoding.ASCII.GetBytes($"{ConfigurationHelper.ClientId}:{ConfigurationHelper.ClientSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",Convert.ToBase64String(byteArry));
            var keyValue=new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };
            var respone=await _httpClient.PostAsync($"{ConfigurationHelper.BaseUrl}/v1/oauth2/token",new FormUrlEncodedContent(keyValue));
            var responeString=await respone.Content.ReadAsStringAsync();
            var AuthoRespone= JsonConvert.DeserializeObject<AuthorizationResponseData>(responeString);
            SetToken(AuthoRespone.access_token);
            return AuthoRespone;
        }

        public void SetToken(string token)
        {
            _httpClient.SetBearerToken(token);
        }

        public async Task<CreateSubscriptionResponse> GetSubscriptionAsync(string subscriptionId)
        {
            var Token=await GetAuthorizationRequestAsync();
            var httpRequestMessage = new HttpRequestMessage (HttpMethod.Get,$"{ConfigurationHelper.BaseUrl}/v1/billing/subscriptions/{subscriptionId}");
            httpRequestMessage.Headers.Authorization=new AuthenticationHeaderValue("Bearer",Token.access_token);
            var respone=await _httpClient.SendAsync(httpRequestMessage);
            var responeAsString=await respone.Content.ReadAsStringAsync();
            if (!respone.IsSuccessStatusCode)
                throw new Exception($"خطأ أثناء إنشاء الخطة - Status: {respone.StatusCode}, Body: {responeAsString}");
            var result=JsonConvert.DeserializeObject<CreateSubscriptionResponse>(responeAsString);
            return result;
        }

        public async Task<bool> ActiveSubscriptionAsync(string subscriptionId)
        {
            var Token = await GetAuthorizationRequestAsync();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{ConfigurationHelper.BaseUrl}/v1/billing/subscriptions/{subscriptionId}/activate");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);
            var respone = await _httpClient.SendAsync(httpRequestMessage);
            var responeAsString = await respone.Content.ReadAsStringAsync();
            if (!respone.IsSuccessStatusCode)
                throw new Exception($"خطأ أثناء إنشاء الخطة - Status: {respone.StatusCode}, Body: {responeAsString}");
            return true;
        }

        public async Task<bool> CancelSubscriptionAsync(string subscriptionId)
        {
            var Token = await GetAuthorizationRequestAsync();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{ConfigurationHelper.BaseUrl}/v1/billing/subscriptions/{subscriptionId}/cancel");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);
            var respone = await _httpClient.SendAsync(httpRequestMessage);
            var responeAsString = await respone.Content.ReadAsStringAsync();
            if (!respone.IsSuccessStatusCode)
                throw new Exception($"خطأ أثناء إنشاء الخطة - Status: {respone.StatusCode}, Body: {responeAsString}");
            return true;
        }

        public async     Task<bool> SuspendSubscriptionAsync(string subscriptionId)
        {
            var Token = await GetAuthorizationRequestAsync();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{ConfigurationHelper.BaseUrl}/v1/billing/subscriptions/{subscriptionId}/suspend");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);
            var respone = await _httpClient.SendAsync(httpRequestMessage);
            var responeAsString = await respone.Content.ReadAsStringAsync();
            if (!respone.IsSuccessStatusCode)
                throw new Exception($"خطأ أثناء إنشاء الخطة - Status: {respone.StatusCode}, Body: {responeAsString}");
            return true;
        }
    }
}
