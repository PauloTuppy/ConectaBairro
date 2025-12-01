using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ConectaBairro.Models;
using ConectaBairro.Models.Entities;
using ConectaBairro.MockData; // For seeding mock data
using System.Text.Json; // For deserializing entity properties

namespace ConectaBairro.Services;

public class DatabaseService

{

    private static SQLiteAsyncConnection? _database;

    private static readonly Lazy<Task<DatabaseService>> _lazyInitializer =

        new Lazy<Task<DatabaseService>>(async () =>

        {

            var instance = new DatabaseService();

            await instance.InitializeDatabaseAsync();

            return instance;

        });



    private DatabaseService() { } // Private constructor to enforce singleton pattern



    public static Task<DatabaseService> Instance => _lazyInitializer.Value;



    public async Task InitializeDatabaseAsync()

    {

        if (_database is not null)

        {

            return;

        }



        var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ConectaBairro.db3");

        _database = new SQLiteAsyncConnection(databasePath);



        // Create tables

        await _database.CreateTableAsync<UserProfileEntity>();

        await _database.CreateTableAsync<CourseEntity>();

        await _database.CreateTableAsync<BadgeEntity>();

        await _database.CreateTableAsync<UserCourseEntity>(); // New linking entity

        await _database.CreateTableAsync<ResourceEntity>(); // New resource entity

        await _database.CreateTableAsync<Alert>(); // New alert entity (alerts table)



        // Seed initial data if tables are empty

        await SeedInitialDataAsync();

    }



    public async Task SeedInitialDataAsync()

    {

        // UserProfile

        if (await _database!.Table<UserProfileEntity>().CountAsync() == 0)

        {

            var userProfile = new UserProfile

            {

                Name = "Paulo Silva",

                Age = 28,

                Location = "São Paulo, SP",

                XP = 450,

                CurrentLevel = 5,

                CreatedAt = DateTime.Now

            }.ToEntity(); // Convert to entity for storage

            await _database.InsertAsync(userProfile);

        }



        // Courses

        if (await _database!.Table<CourseEntity>().CountAsync() == 0)

        {

            var mockCourses = MockCourses.GetMockCourses();

            foreach (var course in mockCourses)

            {

                await _database.InsertAsync(course.ToEntity()); // Convert to entity for storage

            }

        }



        // Badges

        if (await _database!.Table<BadgeEntity>().CountAsync() == 0)

        {

            var mockBadges = MockBadges.GetMockBadges();

            foreach (var badge in mockBadges)

            {

                await _database.InsertAsync(badge.ToEntity()); // Convert to entity for storage

            }

        }



        // Resources (Placeholder for now)

        if (await _database!.Table<ResourceEntity>().CountAsync() == 0)

        {

            var mockResources = new List<ResourceEntity>

            {

                new ResourceEntity { Name = "Biblioteca Municipal", Type = ResourceType.Library, Address = "Rua X, 123", Latitude = -23.5, Longitude = -46.6, Description = "Biblioteca com acervo variado." },

                new ResourceEntity { Name = "CRAS Centro", Type = ResourceType.CommunityCenter, Address = "Av Y, 456", Latitude = -23.6, Longitude = -46.7, Description = "Centro de Referência de Assistência Social." }

            };

            foreach (var resource in mockResources)

            {

                await _database.InsertAsync(resource);

            }

        }

        

        // Alerts (Placeholder for now)

        if (await _database!.Table<Alert>().CountAsync() == 0)

        {

            var mockAlerts = new List<Alert>

            {

                new Alert { Title = "Nova Oportunidade!", Message = "3 vagas abertas no Autonomia e Renda", Type = AlertType.Opportunity, Timestamp = DateTime.Now.AddDays(-1), Location = "Bairro XYZ" },

                new Alert { Title = "Alerta de Chuva", Message = "Previsão de chuva forte na região central", Type = AlertType.Warning, Timestamp = DateTime.Now, Location = "Região Central" }

            };

            foreach (var alert in mockAlerts)

            {

                await _database.InsertAsync(alert);

            }

        }

    }



    // Generic CRUD operations

    public async Task<List<T>> GetItemsAsync<T>() where T : class, new()

    {

        return await _database!.Table<T>().ToListAsync();

    }



    public async Task<T?> GetItemAsync<T>(int id) where T : class, new()

    {

        return await _database!.FindAsync<T>(id);

    }

    

    // Get item by Guid ID for entities that have a Guid PrimaryKey

    public async Task<T?> GetItemByGuidAsync<T>(Guid id) where T : class, new()

    {

        return await _database!.FindAsync<T>(id);

    }





    public async Task<int> SaveItemAsync<T>(T item) where T : class, new()

    {

        // Determine if it's an update or insert based on primary key value

        var pkProperty = typeof(T).GetProperty("Id");

        if (pkProperty == null)

        {

            // No 'Id' property found, always insert

            return await _database!.InsertAsync(item);

        }



        if (pkProperty.PropertyType == typeof(int))

        {

            var id = (int)pkProperty.GetValue(item)!;

            if (id != 0) // Assume 0 for int PK means new item

            {

                return await _database!.UpdateAsync(item);

            }

        }

        else if (pkProperty.PropertyType == typeof(Guid))

        {

            var id = (Guid)pkProperty.GetValue(item)!;

            if (id != Guid.Empty) // Assume Guid.Empty for Guid PK means new item

            {

                // Check if an item with this GUID already exists

                var existingItem = await GetItemByGuidAsync<T>(id); // Use the custom GetItemByGuidAsync

                if (existingItem != null)

                {

                    return await _database!.UpdateAsync(item);

                }

            }

        }

        

        return await _database!.InsertAsync(item);

    }



    public async Task<int> DeleteItemAsync<T>(T item) where T : class, new()

    {

        return await _database!.DeleteAsync(item);

    }

}


