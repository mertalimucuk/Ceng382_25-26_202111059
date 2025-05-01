using System;
using System.Collections.Generic;

namespace Week5Lab.Models
{
    /// <summary>
    /// Represents a student in the system.
    /// </summary>
    public partial class Student
    {
        /// <summary>
        /// Primary key, auto-incremented.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name of the student.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Age of the student (nullable).
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Department the student is enrolled in.
        /// </summary>
        public string? Department { get; set; }
    }
}
