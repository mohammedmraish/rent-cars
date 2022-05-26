import * as _ from 'underscore';
import { SaveVehicle, Vehicle } from './../models/vehicle';
import { VehicleService } from '../services/vehicle.service';
import { Component, Inject, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, forkJoin, observable } from 'rxjs';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css'],
})
export class VehicleFormComponent implements OnInit {
  makes: any[];
  models: any[];
  features: any[];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    contact: {
      contactName: '',
      contactEmail: '',
      contactPhone: '',
    },
    features: [],
  };

  constructor(
    private rout: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private toastr: ToastrService
  ) {
    this.rout.params.subscribe((p) => {
      if (p['id']) this.vehicle.id = +p['id'];
    });
  }

  ngOnInit(): void {
    forkJoin([
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures(),
    ]).subscribe((data) => {
      this.makes = data[0];
      this.features = data[1];
    });

    if (this.vehicle.id)
      this.vehicleService.getVehicle(this.vehicle.id).subscribe(
        (res) => {
          if (res == null) this.router.navigate(['/']);

          this.setVehicle(res);
          this.populateModules();
        },
        (err) => {
          this.router.navigate(['/']);
        }
      );
  }

  private setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features, 'id');
  }

  onMakeChange() {
    this.populateModules();
    delete this.vehicle.modelId;
  }

  private populateModules() {
    var selectedMake = this.makes.find((m) => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }

  onFeaterToggle(featerId, $event) {
    if ($event.target.checked) {
      this.vehicle.features.push(featerId);
    } else {
      var index = this.vehicle.features.indexOf(featerId);
      this.vehicle.features.splice(index, 1);
    }
  }

  onSubmit() {
    if (this.vehicle.id) {
      this.vehicleService.updateVehicle(this.vehicle).subscribe((x) => {
        this.toastr.success('seccsec', 'update vehicle succesfuly');
      });
    } else {
      this.vehicleService.creatVehicle(this.vehicle).subscribe(
        (res) => console.log(res),
        (err) => {
          this.toastr.error('error', 'An expected', {
            progressBar: true,
            closeButton: true,
          });
        }
      );
    }
  }

  onDelete() {
    if (confirm('are you sure?')) {
      this.vehicleService.deleteVehicle(this.vehicle.id).subscribe((x) => {
        this.toastr.success('delete', 'vehicle deleted succesfully');
        this.router.navigate(['/']);
      });
    }
  }
}
