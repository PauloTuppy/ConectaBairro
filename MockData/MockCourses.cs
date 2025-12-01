namespace ConectaBairro.MockData;

using ConectaBairro.Models;

public static class MockCourses
{
    public static List<Course> GetMockCourses()
    {
        return new()
        {
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Técnico em Manutenção Elétrica",
                Description = "Formação técnica em manutenção de sistemas elétricos",
                Provider = "SENAI",
                Program = ProgramType.AutonomiaERenda,
                Duration = "6 meses",
                WeeklyHours = 20,
                Stipend = 858, // R$ 858/mês
                Location = "São Caetano do Sul, SP",
                AvailableVacancies = 45,
                MinEducationRequired = EducationLevel.Medio,
                Areas = new[] { "Técnico", "Industrial", "Elétrica" },
                EnrollmentStartDate = DateTime.Now,
                EnrollmentDeadline = DateTime.Now.AddDays(30),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Elétrica",
                AverageStudentAge = 28
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Operador de Computador",
                Description = "Curso de formação para operador de sistemas informatizados",
                Provider = "SENAC",
                Program = ProgramType.PronatecFIC,
                Duration = "3 meses",
                WeeklyHours = 15,
                Stipend = 0, // Gratuito
                Location = "Rio de Janeiro, RJ",
                AvailableVacancies = 120,
                MinEducationRequired = EducationLevel.Fundamental,
                Areas = new[] { "Informática", "Administrativo" },
                EnrollmentStartDate = DateTime.Now,
                EnrollmentDeadline = DateTime.Now.AddDays(15),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Informática",
                AverageStudentAge = 32
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Auxiliar de Serviços Diversos",
                Description = "Preparação para atuação em serviços gerais",
                Provider = "IFPE",
                Program = ProgramType.AutonomiaERenda,
                Duration = "2 meses",
                WeeklyHours = 16,
                Stipend = 660, // R$ 660/mês
                Location = "Recife, PE",
                AvailableVacancies = 80,
                MinEducationRequired = EducationLevel.Fundamental,
                Areas = new[] { "Serviços", "Limpeza", "Manutenção" },
                EnrollmentStartDate = DateTime.Now.AddDays(-5),
                EnrollmentDeadline = DateTime.Now.AddDays(25),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Serviços",
                AverageStudentAge = 45
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Técnico em Segurança do Trabalho",
                Description = "Especialização em normas e procedimentos de segurança",
                Provider = "IFRS",
                Program = ProgramType.PronatecTecnico,
                Duration = "6 meses",
                WeeklyHours = 25,
                Stipend = 0,
                Location = "Canoas, RS",
                AvailableVacancies = 35,
                MinEducationRequired = EducationLevel.Medio,
                Areas = new[] { "Segurança", "Trabalho", "Administrativo" },
                EnrollmentStartDate = DateTime.Now,
                EnrollmentDeadline = DateTime.Now.AddDays(20),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Segurança",
                AverageStudentAge = 35
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Técnico em Enfermagem",
                Description = "Formação para atuação na área de saúde",
                Provider = "SENAI",
                Program = ProgramType.PronatecTecnico,
                Duration = "12 meses",
                WeeklyHours = 30,
                Stipend = 0,
                Location = "Belo Horizonte, MG",
                AvailableVacancies = 50,
                MinEducationRequired = EducationLevel.Medio,
                Areas = new[] { "Saúde", "Enfermagem", "Hospitalar" },
                EnrollmentStartDate = DateTime.Now.AddDays(10),
                EnrollmentDeadline = DateTime.Now.AddDays(35),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Enfermagem",
                AverageStudentAge = 40
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Soldador (Processos MIG/MAG)",
                Description = "Capacitação em solda com processos modernos",
                Provider = "SENAI",
                Program = ProgramType.AutonomiaERenda,
                Duration = "4 meses",
                WeeklyHours = 20,
                Stipend = 750,
                Location = "Porto Alegre, RS",
                AvailableVacancies = 60,
                MinEducationRequired = EducationLevel.Fundamental,
                Areas = new[] { "Industrial", "Soldagem", "Técnico" },
                EnrollmentStartDate = DateTime.Now,
                EnrollmentDeadline = DateTime.Now.AddDays(18),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Soldagem",
                AverageStudentAge = 30
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Cuidador de Idosos",
                Description = "Formação para cuidados com pessoas idosas",
                Provider = "SENAC",
                Program = ProgramType.PronatecFIC,
                Duration = "3 meses",
                WeeklyHours = 20,
                Stipend = 0,
                Location = "São Paulo, SP",
                AvailableVacancies = 40,
                MinEducationRequired = EducationLevel.Fundamental,
                Areas = new[] { "Saúde", "Cuidados", "Social" },
                EnrollmentStartDate = DateTime.Now,
                EnrollmentDeadline = DateTime.Now.AddDays(22),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Cuidador",
                AverageStudentAge = 38
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Confeiteiro",
                Description = "Curso de confeitaria profissional",
                Provider = "SENAC",
                Program = ProgramType.PronatecFIC,
                Duration = "4 meses",
                WeeklyHours = 16,
                Stipend = 0,
                Location = "Curitiba, PR",
                AvailableVacancies = 25,
                MinEducationRequired = EducationLevel.Fundamental,
                Areas = new[] { "Gastronomia", "Alimentação", "Empreendedorismo" },
                EnrollmentStartDate = DateTime.Now.AddDays(5),
                EnrollmentDeadline = DateTime.Now.AddDays(30),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Confeitaria",
                AverageStudentAge = 35
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Assistente Administrativo",
                Description = "Formação em rotinas administrativas e atendimento",
                Provider = "SESC",
                Program = ProgramType.AutonomiaERenda,
                Duration = "5 meses",
                WeeklyHours = 20,
                Stipend = 900,
                Location = "Salvador, BA",
                AvailableVacancies = 55,
                MinEducationRequired = EducationLevel.Medio,
                Areas = new[] { "Administrativo", "Atendimento", "Escritório" },
                EnrollmentStartDate = DateTime.Now,
                EnrollmentDeadline = DateTime.Now.AddDays(25),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Administrativo",
                AverageStudentAge = 28
            },
            new Course
            {
                Id = Guid.NewGuid(),
                Name = "Técnico em Logística",
                Description = "Gestão de estoque, transporte e distribuição",
                Provider = "SENAI",
                Program = ProgramType.PronatecTecnico,
                Duration = "8 meses",
                WeeklyHours = 25,
                Stipend = 0,
                Location = "Campinas, SP",
                AvailableVacancies = 30,
                MinEducationRequired = EducationLevel.Medio,
                Areas = new[] { "Logística", "Transporte", "Gestão" },
                EnrollmentStartDate = DateTime.Now.AddDays(10),
                EnrollmentDeadline = DateTime.Now.AddDays(40),
                ThumbnailUrl = "https://via.placeholder.com/300x200?text=Logística",
                AverageStudentAge = 32
            }
        };
    }
}
