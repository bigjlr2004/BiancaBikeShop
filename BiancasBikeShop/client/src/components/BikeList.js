import { useState, useEffect } from "react"
import BikeCard from "./BikeCard"
import { getBikes } from "../bikeManager";

export default function BikeList({ setDetailsBikeId }) {
    const [bikes, setBikes] = useState([])

    const getAllBikes = () => {
        getBikes().then(data => setBikes(data));
    }

    useEffect(() => {
        getAllBikes()
    }, [])
    return (
        <>
            <h2>Bikes</h2>
            {bikes.map((bike) => (

                <BikeCard key={bike.id} bike={bike} setDetailsBikeId={setDetailsBikeId} />))}
        </>
    )
}