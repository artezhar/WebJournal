﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebZhurnal.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен состоять хот бы из {2} символов", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль повторно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name ="Тип")]
        [EnumDataType(typeof(UserType))]
        public UserType Type { get;set;}

        [Display(Name ="Предмет")]
        public string Subject { get; set; }

        [Display(Name = "Класс")]
        public string Group { get; set; }

    }
}
