import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ClfService } from 'src/app/service/clf.service';

@Component({
  selector: 'app-clf-batch-dialog',
  templateUrl: './clf-batch-dialog.component.html',
  styleUrls: ['./clf-batch-dialog.component.css']
})
export class ClfBatchDialogComponent implements OnInit {

  formData: FormData = new FormData();

  constructor(
    private clfService: ClfService,
    public dialogRef: MatDialogRef<ClfBatchDialogComponent>
  ) { }

  ngOnInit() {
  }

  onSelectedFile(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.formData.append('file', file);
    }
  }

  onSubmit() {
    console.log(this.formData);
    this.clfService.batchClf(this.formData).subscribe(result => { });
  }

  cancel(): void {
    this.dialogRef.close();
    //this.clfForm.reset();
  }

}
