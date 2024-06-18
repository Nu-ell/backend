using Azure.Core;
using Azure;
using TechDictionaryApi.DTOs;
using TechDictionaryApi.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TechDictionaryApi.Services
{
    public class UserRequestService : IUserRequestService
    {
        private readonly IUserRequestRepository _userRequestRepository;

        public UserRequestService(IUserRequestRepository userRequestRepository)
        {
            _userRequestRepository = userRequestRepository;
        }

        public async Task<string> RequestChangeToWordorRequestNewWord(UserRequestDTO request)
        {
            try
            {
                string response = string.Empty;

                if (string.IsNullOrEmpty(request.Word) || string.IsNullOrEmpty(request.RequestDefinitionOrDescription) || string.IsNullOrEmpty(request.CreatedBy))
                {
                    response = "Ensure all fields are inputted correctly";
                    return response;
                }

                if (request.UserRequestTypeId <= 0 || request.UserRequestTypeId > 2) //UserRequestTypeId: 1 is "RequestChangeToWord" while 2 is "RequestNewWord".
                {
                    response = "Invalid UserRequestTypeId was supplied";
                    return response;
                }

                int resp = await _userRequestRepository.RequestChangeToWordorRequestNewWord(request);
                if (resp > 0)
                {
                    response = "Your request has been submitted successfully";
                    return response;
                }
                else
                {
                    response = "An error occured while submitting request. Kindly contact admin.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<string> ResolveRequest(long userRequestId, string resolvedBy)
        {
            try
            {
                string response = string.Empty;

                if (userRequestId <= 0)
                {
                    response = "UserRequestId is required";
                    return response;
                }

                if (string.IsNullOrEmpty(resolvedBy))
                {
                    response = "ResolvedBy is required.";
                    return response;
                }

                int resp = await _userRequestRepository.ResolveRequest(userRequestId, resolvedBy);
                if (resp > 0)
                {
                    response = "Your request has been submitted successfully";
                    return response;
                }
                else
                {
                    response = "An error occured while submitting request. Kindly contact admin.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UserRequestListDTO>> GetAllUserRequests()
        { 
            try
            {
                var userRequests = await _userRequestRepository.GetAllUserRequests();
                if (userRequests.Any())
                {                   
                    return userRequests;
                }
                else
                {
                    //response = "No User Requests found.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                var error = $"Exception Occured: {ex.Message}";
                return null;
            }
        }

        public async Task<GeneralUserDashboardResponseDTO> GetGeneralUserDashboard()
        {
            try
            {
                var dashboardDetails = await _userRequestRepository.GetGeneralUserDashboard();
                if (dashboardDetails != null)
                {
                    return dashboardDetails;
                }
                else
                {
                    //response = "No Dashboard Details found.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                var error = $"Exception Occured: {ex.Message}";
                return null;
            }
        }
    }
}
