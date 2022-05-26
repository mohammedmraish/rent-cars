import { VehicleService } from './../services/vehicle.service';
import { Vehicle, KeyValuePair } from './../models/vehicle';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
})
export class VehicleListComponent implements OnInit {
  vehicles: Vehicle[];
  allvehicles: Vehicle[];
  makes: KeyValuePair[];
  filter: any = {};

  constructor(private vehicleService: VehicleService) {
    vehicleService.getAllVehicles().subscribe((res) => {
      this.vehicles = res;
      this.allvehicles = res;
    });

    vehicleService.getMakes().subscribe((res) => {
      this.makes = res;
    });
  }

  onFilterChange() {
    console.log('chang');

    var vehicles = this.allvehicles;

    if (this.filter.makeId)
      vehicles = vehicles.filter((v) => v.make.id == this.filter.makeId);

    this.vehicles = vehicles;
  }

  resetFilter() {
    this.filter = {};
    this.onFilterChange();
  }
  ngOnInit(): void {}
}
