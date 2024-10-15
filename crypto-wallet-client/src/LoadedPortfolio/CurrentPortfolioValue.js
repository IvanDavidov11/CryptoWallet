import React from 'react';
import { useState, useEffect } from 'react';

const CurrentPortfolioValue = () => {
    const calculateCurrent_ApiUrl="https://localhost:7038/api/calc/current";
    const [currentValue, setCurrentValue] = useState(0);

    useEffect (() => {
        const fetchCurrentValue = async () => {
            try {
              const response = await fetch(calculateCurrent_ApiUrl);
    
              if (!response.ok) throw Error('Did not receive expected data');
    
              const currentValueInJson = await response.json();
              setCurrentValue(parseFloat(currentValueInJson));
            }
            catch (err) {
              console.log(err);
            }
          }
    
          fetchCurrentValue();
    },[]);

    return (
        <div>
            <h3>Current Value:</h3>
            <h4>{currentValue}</h4>
        </div>
    )
}

export default CurrentPortfolioValue