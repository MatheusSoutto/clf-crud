import { Component, Input, OnInit } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Clf } from 'src/app/model/clf.model';
import { ClfService } from 'src/app/service/clf.service';
import { ClfFormDialogComponent } from 'src/app/shared/clf-form-dialog/clf-form-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ])
  ]
})
export class TableComponent implements OnInit {

  @Input()
  dataSource: Clf[];

  columnsToDisplay = ['client', 'rfcIdentity', 'userId', 'method', 'request',
                      'statusCode', 'responseSize', 'requestDate', 'referrer', 'actions'];

  constructor(
    public clfService: ClfService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    
  }

  updateClf(clf: Clf): void {
    console.log(clf);
    const dialogRef = this.dialog.open(ClfFormDialogComponent, {
      minWidth: '400px',
      data: clf
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  deleteClf(id: string): void {
    this.clfService.deleteClf(id).subscribe(result => { });
    this.dataSource = null;
  }

}
