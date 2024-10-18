import React from 'react';
import { useState, useEffect } from 'react';

const InitialPortfolioValue = () => {
  const calculateInitial_ApiUrl = "https://localhost:7038/api/calc/initial";
  const [initialValue, setInitialValue] = useState(0);

  useEffect(() => {
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
  }, []);

  return (
    <div className='portfolio-infobox'>
      <p>Initial Value:</p>
      <p><strong>${initialValue.toFixed(2)}</strong></p>
    </div>
  )
}

export default InitialPortfolioValue