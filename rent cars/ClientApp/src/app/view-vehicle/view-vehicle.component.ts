import { photoService } from './../services/photo.service';
import { ToastrService } from 'ngx-toastr';

import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleService } from '../services/vehicle.service';

@Component({
  templateUrl: 'view-vehicle.component.html',
})
export class ViewVehicleComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  vehicle: any;
  vehicleId: number;
  photos: any[];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toasty: ToastrService,
    private vehicleService: VehicleService,
    private photoService: photoService
  ) {
    route.params.subscribe((p) => {
      this.vehicleId = +p['id'];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
        router.navigate(['/vehicles']);
        return;
      }
    });
  }

  ngOnInit() {
    this.photoService
      .getPhotos(this.vehicleId)
      .subscribe((res) => (this.photos = res));

    this.vehicleService.getVehicle(this.vehicleId).subscribe(
      (v) => (this.vehicle = v),
      (err) => {
        if (err.status == 404) {
          this.router.navigate(['/vehicles']);
          return;
        }
      }
    );
  }

  delete() {
    if (confirm('Are you sure?')) {
      this.vehicleService.deleteVehicle(this.vehicle.id).subscribe((x) => {
        this.router.navigate(['/vehicles']);
      });
    }
  }

  uploudePhoto() {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    this.photoService
      .upload(this.vehicle.id, nativeElement.files[0])
      .subscribe((x) => console.log(x));
  }
}
