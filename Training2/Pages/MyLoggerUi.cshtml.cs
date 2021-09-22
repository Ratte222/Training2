using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AuxiliaryLib.Helpers;
using DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Training2.Pages
{
    public class MyLoggerUiModel : PageModel
    {
        public PageResponse<Model.MyLoggerUiModel> PageResponse { get; set; } = null;
        public Model.MyLoggerUiModel TempModel { get; set; } = new Model.MyLoggerUiModel();
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public MyLoggerUiModel(AppDBContext context, IMapper mapper)
        {
            (_context, _mapper) = (context, mapper);
        }

        public void OnGet()
        {
            if(PageResponse is null)
            {
                PageResponse = new PageResponse<Model.MyLoggerUiModel>();
            }
            PageResponse.Items = _mapper.ProjectTo<Model.MyLoggerUiModel>(
                _context.Logs.Skip(PageResponse.Skip).Take(PageResponse.Take)).ToList();
        }
    }
}
