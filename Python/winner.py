import argparse
import subprocess
from peaceful_pie.unity_comms import UnityComms

def run(args):
    unity_comms = UnityComms(port=9000)
    unity_comms.WhoWon(winner=str(args.winner))

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Send the winner of the game to the Unity game.")
    parser.add_argument("--winner", type=str, help="The winner of the game.")
    args = parser.parse_args()
    run(args)