Project Customer Feedback System merupakan sebuah project agar customer bisa mengirimkan feedback ke system.
Project ini terdiri dari 3 tabel:
1. Customers
    CustomerID (Primary Key)
    Name
    Email
2. Feedback
    FeedbackID (Primary Key)
    CustomerID (Foreign Key)
    FeedbackText
    Status (Pending, Reviewed)
3. Audit (Trigger jika Feedback.Status di Update)
   id (Primary Key)
   FeedbackID(Foreign Key)
   Now
   Last
   Tabel
   Kolom
   Tanggal

Project ini terdiri dari :
1. Feedback Submission (Console App): untuk menambahkan customer dan feedback dari console
2. Feedback Review (ASP.NET Core): Inteface untuk mengubah status dari pending ke Reviewed.
3. API : ASP.NET mengambil data dari API
4. Feedback Status Update (SQL Trigger): Trigger to update feedback status when reviewed.
5. Weekly Reminder (Hangfire): Hangfire job to send weekly reminders to managers to review
pending feedback.

<img width="569" alt="image" src="https://github.com/rholly4502/Mini_Project_CustomerFeedbackSystem/assets/134238845/c6f4f8a6-29ac-4e7a-aa38-805db3ba8fa6">
<img width="893" alt="image" src="https://github.com/rholly4502/Mini_Project_CustomerFeedbackSystem/assets/134238845/1ee9eaf3-5617-404c-9988-f1eaa466d441">
<img width="891" alt="image" src="https://github.com/rholly4502/Mini_Project_CustomerFeedbackSystem/assets/134238845/e7473c1f-dee3-4719-be8f-8665df161e7c">
<img width="893" alt="image" src="https://github.com/rholly4502/Mini_Project_CustomerFeedbackSystem/assets/134238845/f3f8818d-11cb-4471-a905-e8faf0ffcdab">



