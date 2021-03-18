import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-clf-form-dialog',
  templateUrl: './clf-form-dialog.component.html',
  styleUrls: ['./clf-form-dialog.component.css']
})
export class ClfFormDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ClfFormDialogComponent>
  ) { }

  ngOnInit() {
  }

  cancel(): void {
    this.dialogRef.close();
  }

}
