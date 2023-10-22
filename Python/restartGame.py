from peaceful_pie.unity_comms import UnityComms
import argparse

def run(args):
    unity_comms = UnityComms(port=9000)
    res = unity_comms.RestartGame()
    
    
if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    #parser.add_argument('--message', type=str, required = True)
    args = parser.parse_args()
    run(args)