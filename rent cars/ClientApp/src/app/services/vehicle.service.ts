import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  constructor(private http: HttpClient) {}

  getMakes() {
    return this.http.get<any>('https://localhost:7275/api/makes');
  }

  getFeatures() {
    return this.http.get<any>('https://localhost:7275/api/features');
  }

  creatVehicle(vehicle) {
    return this.http.post('https://localhost:7275/api/vehicles', vehicle);
  }

  getVehicle(id) {
    return this.http.get<any>('https://localhost:7275/api/vehicles/' + id);
  }

  updateVehicle(vehicle) {
    return this.http.put(
      'https://localhost:7275/api/vehicles/' + vehicle.id,
      vehicle
    );
  }

  deleteVehicle(id) {
    return this.http.delete('https://localhost:7275/api/vehicles/' + id);
  }

  getAllVehicles() {
    return this.http.get<any>('https://localhost:7275/api/vehicles');
  }
}
