import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ClfService } from 'src/app/service/clf.service';

@Component({
  selector: 'app-clf-form-dialog',
  templateUrl: './clf-form-dialog.component.html',
  styleUrls: ['./clf-form-dialog.component.css']
})
export class ClfFormDialogComponent implements OnInit {
  clfForm: FormGroup;

  method: string = 'GET';

  methodOptions: string[] = ['GET', 'POST', 'PUT', 'DELETE', 'PATCH', 'OPTIONS',
                          'HEAD', 'CONNECT', 'TRACE'];

  timeZone: string = '+00:00';

  timeZoneList: string[] = ['-12:00', '-11:00', '-10:00', '-09:00', '-08:00', '-07:00', '-06:00', '-05:00', 
                      '-04:00', '-03:00', '-02:00', '-01:00', '+00:00', '+01:00', '+02:00', '+03:00', 
                      '+04:00', '+05:00', '+06:00', '+07:00', '+08:00', '+09:00', '+10:00', '+11:00', '+12:00'];

  constructor(
    private formBuilder: FormBuilder,
    private clfService: ClfService,
    public dialogRef: MatDialogRef<ClfFormDialogComponent>
  ) { }

  ngOnInit() {
    this.clfForm = this.formBuilder.group({
      client: ['', Validators.required],
      rfcIdentity: ['', Validators.required],
      userId: ['', Validators.required],
      requestDate: ['', Validators.required],
      time: ['', Validators.required],
      timeZone: ['', Validators.required],
      method: ['', Validators.required],
      request: ['', Validators.required],
      protocol: ['', Validators.required],
      statusCode: ['', Validators.required],
      responseSize: ['', Validators.required],
      referrer: ['', Validators.required],
      userAgent: ['', Validators.required]
    });
  }

  createClf(): void {
    this.clfService.postClf(this.clfForm.value).subscribe(result => {});
    this.dialogRef.close();
    this.clfForm.reset();
  }

  cancel(): void {
    this.dialogRef.close();
    this.clfForm.reset();
  }

}
