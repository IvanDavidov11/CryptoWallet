import React from 'react';
import { useState, useEffect } from 'react';

const InitialPortfolioValue = () => {
    const calculateInitial_ApiUrl="https://localhost:7038/api/calc/initial";
    const [initalValue, setInitialValue] = useState(0);

    useEffect (() => {
        const fetchInitalValue = async () => {
            try {
              const response = await fetch(calculateInitial_ApiUrl);
    
              if (!response.ok) throw Error('Did not receive expected data');
    
              const initialValueJson = await response.json();
              setInitialValue(parseFloat(initialValueJson));
            }
            catch (err) {
              console.log(err);
            }
          }
    
          fetchInitalValue();
    },[]);


    return (
        <div>
            <h3>Initial Value:</h3>
            <h4>{initalValue}</h4>
        </div>
    )
}

export default InitialPortfolioValue
