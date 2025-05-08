using System;
using System.Collections.Generic;

namespace Week5Lab.Models
{
    
    /// Represents a student in the system.
    
    public partial class Student
    {
        
        /// Primary key, auto-incremented.
       
        public int Id { get; set; }

       
        /// Full name of the student.
      
        public string Name { get; set; } = string.Empty;

       
        /// Age of the student (nullable).
        
        public int? Age { get; set; }

       
        /// Department the student is enrolled in.
        
        public string? Department { get; set; }
    }
}
