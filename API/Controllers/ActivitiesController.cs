using System;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

public class ActivitiesController(AppDbContext context) : BaseApiController
{
    //primary constructor injection
    private readonly AppDbContext _context = context;

    [HttpGet]   
    public async Task<ActionResult<List<Activity>>> GetActivities()
    {
        return await _context.Activities.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivityDetail(string id)
    {
        var activity =  await _context.Activities.FindAsync(id);
        if (activity == null)
        {
            return NotFound();
        }
        return activity;
    }
}
// Compare this snippet from API/Controllers/BaseApiController.cs:
// using Microsoft.AspNetCore.Mvc;  