import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as moment from 'moment';
import { UtilService } from 'src/app/helper/util';
import { Clf } from 'src/app/model/clf.model';
import { ClfService } from 'src/app/service/clf.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  clfForm: FormGroup;
  client: string = '';
  userAgent: string;

  requestDate: moment.Moment = moment();
  time: string = '00:00:00';
  timeZone: string = '+00:00';

  timeZoneList: string[] = ['-12:00', '-11:00', '-10:00', '-09:00', '-08:00', '-07:00', '-06:00', '-05:00', 
                      '-04:00', '-03:00', '-02:00', '-01:00', '+00:00', '+01:00', '+02:00', '+03:00', 
                      '+04:00', '+05:00', '+06:00', '+07:00', '+08:00', '+09:00', '+10:00', '+11:00', '+12:00'];

  
  clfs: Clf[];

  constructor(
    private formBuilder: FormBuilder,
    public clfService: ClfService,
    private utilService: UtilService
  ) { }

  ngOnInit() {
    this.clfForm = this.formBuilder.group({
      client: ['', [Validators.required]],
      requestDate: [this.requestDate, [Validators.required]],
      time: [this.time, [Validators.required]],
      timeZone: [this.timeZone, [Validators.required]],
      userAgent: ['', [Validators.required]]
    });
  }

  eventSelection(event: string){
    this.timeZone = event;
  }

  onSubmitClient() {
    this.getClfsByClient(this.clfForm.controls.client.value);
  }

  onSubmitRequestDate() {
    this.requestDate = this.utilService.setTimeAndZone(this.clfForm.controls.requestDate.value,
                                                        this.clfForm.controls.time.value,
                                                        this.clfForm.controls.timeZone.value);

    this.getClfsByRequestDate(this.requestDate.creationData().input.toString());
  }

  onSubmitUserAgent() {
    debugger;
    this.getClfsByUserAgent(this.clfForm.controls.userAgent.value.toString());
  }

  getClfs() {
    this.clfService.getClfs().subscribe(data => {
      this.clfs = data;
    });
  }

  getClfsByClient(client: string) {
    this.clfService.getClfsByClient(client).subscribe(data => {
      this.clfs = data;
    });
  }

  getClfsByRequestDate(requestDate: string) {
    this.clfService.getClfsByRequestDate(requestDate).subscribe(data => {
      this.clfs = data;
    });
  }

  getClfsByUserAgent(userAgent: string) {
    this.clfService.getClfsByUserAgent(userAgent).subscribe(data => {
      this.clfs = data;
    });
  }

}
