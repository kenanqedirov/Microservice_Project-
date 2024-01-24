using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        public readonly IMongoCollection<Course> _courseCollection;
        public readonly IMongoCollection<Category> _categoryCollection;
        private readonly IPublishEndpoint _publishEndpoint;
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Shared.Dtos.Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Shared.Dtos.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Shared.Dtos.Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return Shared.Dtos.Response<CourseDto>.Fail("Course not found", 404);
            }
            course.Category =await _categoryCollection.Find<Category>(a=>a.Id == course.CategoryId).FirstAsync();
            
            return Shared.Dtos.Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Shared.Dtos.Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x=>x.UserId == userId).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category =await _categoryCollection.Find<Category>(a=>a.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Shared.Dtos.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Shared.Dtos.Response<CourseDto>> CreateAsync (CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);
            return Shared.Dtos.Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Shared.Dtos.Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(a => a.Id == courseUpdateDto.Id, updateCourse);
            if (result == null)
            {
                return Shared.Dtos.Response<NoContent>.Fail("Course not found", 404);
            }
           
            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent
            {
                CourseId =updateCourse.Id,UpdatedName = updateCourse.Name 
            });
            return Shared.Dtos.Response<NoContent>.Success(200);
        } 

        public async Task<Shared.Dtos.Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(a=>a.Id == id);
            if (result.DeletedCount > 0)
            {
                return Shared.Dtos.Response<NoContent>.Success(200);
            }
            else
            {
                return Shared.Dtos.Response<NoContent>.Fail("Course not found",404);
            }
        }
    }
}