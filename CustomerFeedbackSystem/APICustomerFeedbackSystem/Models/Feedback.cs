﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace APICustomerFeedbackSystem.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int CustomerId { get; set; }

    public string FeedbackText { get; set; }

    public string Status { get; set; }

    public virtual Customer Customer { get; set; }
}