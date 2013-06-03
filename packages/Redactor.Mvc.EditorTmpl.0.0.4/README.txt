Steps to use,

- You should add UIHint attribute which property you want to use like following sample

   public class Book
   {
      [Key]
      public int Isbn { get; set; }
      [UIHint("Redactor")]                                  /*When you istalling Redactor.Mvc.EditorTmpl package, Nuget will create editor template in Views/Shared/EditorTemplates/Redactor.cshtml for you. */
      [Required]
      public string Title { get; set; }
      public DateTime FirstPublished { get; set; }
      public bool IsFiction { get; set; }
   }  
    
 - Sample View,  Create.cshtml:
 
    <div class="editor-label">
         @Html.LabelFor(model => model.Title)
    </div>
    <div class="editor-field">
         @Html.EditorFor(model => model.Title)              /* You can continue to write @Html.EditorFor() method for your text property. */
         @Html.ValidationMessageFor(model => model.Title)
    </div>
    
 - Thats all..

 
 