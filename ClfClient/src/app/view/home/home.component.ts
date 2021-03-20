import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ClfBatchDialogComponent } from 'src/app/shared/clf-batch-dialog/clf-batch-dialog.component';
import { ClfFormDialogComponent } from 'src/app/shared/clf-form-dialog/clf-form-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(
    public dialog: MatDialog
  ) { }

  ngOnInit() {
  }

  addClf(): void {
    const dialogRef = this.dialog.open(ClfFormDialogComponent, {
      minWidth: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  batchClf(): void {
    const dialogRef = this.dialog.open(ClfBatchDialogComponent, {
      minWidth: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

}
