using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ClinicDTOs;
using BusinessObject.DTO.ClinicDTOs.View;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;

namespace Services.Impl;

public class ClinicService : IClinicService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public ClinicService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ClinicDTO> GetClinic(Guid id)
    {
        var clinicModel = await _unitOfWork.ClinicRepository.GetClinic(id);
        return _mapper.Map<ClinicDTO>(clinicModel);
        //return clinicModel;
    }
 
    public async Task<List<ClinicDTO>> GetClinics(string? filterOn , string? filterQuery, string? sortBy, bool isAscending )
    {
        List<Clinic> list = await _unitOfWork.ClinicRepository.GetClinics(filterOn, filterQuery, sortBy, isAscending);
        var listDto =  _mapper.Map<List<ClinicDTO>>(list);
        return listDto;
    }
    public async Task<ResponseDTO> GetAllClinics(int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField,
        string? sortOrder)
    {
        try
        {
            List<Clinic> models = await _unitOfWork.ClinicRepository.GetAllClinics(pageNumber, rowsPerPage);
                
            // Filter 
            if (!string.IsNullOrEmpty(filterField) || !string.IsNullOrEmpty(filterValue))
            {
                switch (filterField.ToLower())
                {
                    case "name":
                        models = models.Where(u => u.ClinicName.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                    case "phone":
                        models = models.Where(u =>
                            u.PhoneNumber.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                    case "email":
                        models = models.Where(u => u.Email.Contains(filterValue, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                }
            }
            // Sort
            if (!string.IsNullOrEmpty(sortField) || !string.IsNullOrEmpty(sortOrder))
            {
                bool isAscending = sortOrder.ToLower() == "asc";
                switch (sortField.ToLower())
                {
                    case "name":
                        models = isAscending ? models.OrderBy(u => u.ClinicName).ToList() : models.OrderByDescending(u => u.ClinicName).ToList();
                        break;
                    case "phone":
                        models = isAscending ? models.OrderBy(u => u.PhoneNumber).ToList() : models.OrderByDescending(u => u.PhoneNumber).ToList();
                        break;
                    case "email":
                        models = isAscending ? models.OrderBy(u => u.Email).ToList() : models.OrderByDescending(u => u.Email).ToList();
                        break;
                }
            }
            List<ClinicDTO> viewModels = _mapper.Map<List<ClinicDTO>>(models);
            
            return new ResponseDTO("Get customer successfully!", 200, true, viewModels);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(ex.Message, 500, false, null);
        }
    }

    public async Task<ResponseDTO> CreateClinic(ClinicCreateDTO clinicDto)
    {
        try
        {
            if (clinicDto == null)
            {
                return new ResponseDTO("Clinic should not be empty", 400, false, null);
            }

            Clinic model = _mapper.Map<Clinic>(clinicDto);
            if (!clinicDto.Services.IsNullOrEmpty())
            {
                foreach (var serviceDto in clinicDto.Services)
                {
                    Service? serviceAdding = await _unitOfWork.serviceRepo.GetByIdAsync(serviceDto.ServiceId);
                    if (serviceAdding!= null)
                    {
                        BusinessObject.Entity.ClinicService clinicService = new BusinessObject.Entity.ClinicService()
                        {
                            Service = serviceAdding,
                            Status = true,
                            Price = serviceAdding.Price
                        };
                        model.ClinicServices.Add(clinicService);
                    } else return new ResponseDTO("This Service is not exist!", 400, false, serviceAdding);
                }
            }
            await _unitOfWork.ClinicRepository.CreateClinic(model);
            return new ResponseDTO("Medicine Delete successfully!", 201, true, _mapper.Map<ClinicDTO>(model));
        }
        catch (Exception ex)
        {
            return new ResponseDTO(ex.Message, 500, false, null);
        }
    }

    public async Task<ResponseDTO> DeleteClinic(Guid id)
    {
        try
        {
            Clinic? model = await _unitOfWork.ClinicRepository.GetByIdAsync(id);
            if (model == null)
            {
                return new ResponseDTO("This Clinic is not exist!", 400, false, null);
            }
            
            await _unitOfWork.ClinicRepository.DeleteAsync(model);
            return new ResponseDTO("Medicine Delete successfully!", 201, true, _mapper.Map<ClinicDTO>(model));
        }
        catch (Exception ex)
        {
            return new ResponseDTO(ex.Message, 500, false, null);
        }
    }

    public async Task<ResponseDTO> UpdateClinic(ClinicUpdateDTO requestDto)
    {
        try
        {
            Clinic? model = await _unitOfWork.ClinicRepository.GetByIdAsync(requestDto.ClinicId);
            if (model == null)
            {
                return new ResponseDTO("This Clinic is not exist!", 400, false, null);
            }

            model = _mapper.Map<Clinic>(requestDto);
            if (!requestDto.Services.IsNullOrEmpty())
            {
                foreach (var serviceDto in requestDto.Services)
                {
                    Service? serviceAdding = await _unitOfWork.serviceRepo.GetServiceByID(serviceDto.ServiceId);
                    if (serviceAdding!= null)
                    {
                        BusinessObject.Entity.ClinicService clinicService = new BusinessObject.Entity.ClinicService()
                        {
                            Clinic = model,
                            ClinicId = model.ClinicId,
                            ServiceId = serviceAdding.ServiceId,
                            Service = serviceAdding,
                            Status = true,
                            Price = serviceAdding.Price
                        };
                        model.ClinicServices.Add(clinicService);
                    } else return new ResponseDTO("This Service is not exist!", 400, false, serviceAdding);
                }
            }
            model = await _unitOfWork.ClinicRepository.UpdateClinic(model);
            return new ResponseDTO("Clinic Update Successfully!", 201, true, _mapper.Map<ClinicDTO>(model));
        }
        catch (Exception ex)
        {
            return new ResponseDTO(ex.Message, 500, false, null);
        }
    }

    public async Task<ClinicDTO> GetClinicByUserId(Guid userId)
    {
        Clinic userClinic = await _unitOfWork.ClinicRepository.GetClinicByUserId(userId);
        var viewModel = _mapper.Map<ClinicDTO>(userClinic);
        return viewModel;
    }
}